import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {AssignmentStatus, StudentAssignmentDto, StudentAssignmentsService} from "../../../../../clients/teacher";
import {BaseComponent} from "../../../../../core/base-component";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-student-assignment-list',
  templateUrl: './student-assignment-list.component.html',
  styleUrls: ['./student-assignment-list.component.scss']
})
export class StudentAssignmentListComponent extends BaseComponent implements OnInit, OnChanges {

  @Input() items: StudentAssignmentDto[];
  @Input() showStudent: boolean = true;
  @Input() onlyNew: boolean = true;
  @Output() selected: StudentAssignmentDto;
  @Output() selectedChange: EventEmitter<StudentAssignmentDto> = new EventEmitter<StudentAssignmentDto>();

  page = 0;
  pageSize = 30;
  isLastPage: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private studentAssignmentsService: StudentAssignmentsService
  ) {
    super();
  }

  ngOnInit() {
    this.getData();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes["onlyNew"] && !changes["onlyNew"].isFirstChange()){
      this.getData(true);
    }
  }

  getData(reset: boolean = false){
    if (reset) {
      this.loading = true;
      this.items = [];
      this.page = 0;
    }
    ++this.page;
    if (this.page != 1) this.loading = true;
    if (!this.items) this.items = [];
    this.studentAssignmentsService.getAll(this.onlyNew).subscribe(
      data =>{
        this.items = this.items.concat(data);
        this.isLastPage = data.length < this.pageSize;
        this.loading = false;
      },
      err => this.loading = false
    );
  }

  onAssignmentClick(assignment: StudentAssignmentDto) {
    this.selected = assignment;
    this.selectedChange.emit(assignment);
  }

  getStatusIcon(status: AssignmentStatus): string {
    switch (status) {
      case 0:
        return 'bulb-outline'
      case 1:
        return 'clock-outline'
      case 2:
        return 'bulb-outline'
      case 3:
        return 'checkmark-circle-2-outline'

    }
  }

  getTooltipText(status: AssignmentStatus) {
    switch (status) {
      case AssignmentStatus.NUMBER_0:
        return 'Новая'
      case AssignmentStatus.NUMBER_1:
        return 'Вы запросили изменения'
      case AssignmentStatus.NUMBER_2:
        return 'На проверку: студент внес изменения'
      case AssignmentStatus.NUMBER_3:
        return 'Проверено'

    }
  }
}
