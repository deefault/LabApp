/**
 * Teacher api
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

export interface TeacherRegisterDto { 
    email: string;
    phoneCode: number;
    phone: number;
    password: string;
    contactEmail?: string;
    name: string;
    surname: string;
    middlename?: string;
    dateBirth?: Date;
    academicRankId?: number;
}