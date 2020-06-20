using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Services
{
    public class GroupService
    {
        private readonly AppDbContext _db;
        private readonly IUserService _userService;

        private static readonly Random _random = new Random();

        public GroupService(AppDbContext db, IUserService userService)
        {
            _db = db;
            _userService = userService;
        }


        public IEnumerable<Group> Get(Teacher teacher)
        {
            return _db.Groups.Where(x => x.TeacherId == teacher.UserId);
        }

        public IEnumerable<Group> Get(int studentId, params Expression<Func<StudentGroup, object>>[] includes)
        {
            return _db.StudentGroup.Where(x => x.StudentId == studentId).AddIncludes(includes).Select(x => x.Group);
        }

        public async Task<Group> GetByIdAsync(int id, params Expression<Func<Group, object>>[] includes)
        {
            return await _db.Groups.AddIncludes(includes).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> GetByCodeAsync(string code, params Expression<Func<Group, object>>[] includes)
        {
            code = code.ToUpperInvariant();
            return await _db.Groups.AddIncludes(includes).SingleOrDefaultAsync(x => x.InviteCode == code);
        }

        public void AddStudent(Student student, Group @group)
        {
            _db.StudentGroup.Add(new StudentGroup {StudentId = student.UserId, GroupId = group.Id});
        }

        public Group Add(Group @group)
        {
            if (string.IsNullOrEmpty(group.InviteCode)) group.InviteCode = GenerateInviteCode();
            return _db.Groups.Add(@group).Entity;
        }

        private string GenerateInviteCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code;
            do
            {
                code = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            } while (GetByCodeAsync(code).GetAwaiter().GetResult() != null);

            return code;
        }

        public bool StudentExists(int studentId, int groupId)
        {
            return _db.StudentGroup.Any(x => x.StudentId == studentId && x.GroupId == groupId);
        }
        
        public bool StudentExists(int studentId, Subject subject)
        {
            return _db.StudentGroup.Any(x => x.StudentId == studentId && x.Group.SubjectId == subject.Id);
        }
        
        /// <remarks> Use only if current user is student</remarks>>
        public bool StudentExists(Subject subject)
        {
            return StudentExists(_userService.UserId, subject);
        }
        
        

        public async Task<IEnumerable<Group>> GetGroupsAsync(Student student, Paging paging = null)
        {
            return await _db.StudentGroup.Where(x => x.StudentId == student.UserId)
                .Include(x => x.Group.Subject)
                .Include(x => x.Group.Teacher)
                .Select(x => x.Group)
                .ToPagedListAsync(paging);
        }
    }
}