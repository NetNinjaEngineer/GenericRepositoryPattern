﻿using ApplyingGenericRepositoryPattern.DTO;
using ApplyingGenericRepositoryPattern.Entities;
using ApplyingGenericRepositoryPattern.Helpers;

namespace ApplyingGenericRepositoryPattern.Repository;

public interface IFeatureService
{
    Task<IEnumerable<ShowStudentsWithCoursesRegisteredModel>>
        GetStudentsWithCoursesRegistered();

    Task<IEnumerable<CoursesWithDepartmentsAndPreRequestsModel>>
       GetCoursesWithDepartmentsAndPreRequests();

    Task<IEnumerable<CoursesWithPreRequestsModel>>
        GetCoursesWithPreRequests();

    Task<(string, string)> AssignCourseToStudent(int courseId, int studentId);

    Task<bool> CheckValidIDS(int courseId, int studentId);

    Task<(bool, string)> CheckPreRequestCourse(int courseId, int studentId);

    Task<bool> CheckCourseHaveBeenEnrolled(int courseId, int studentId);

    Task<IEnumerable<CoursesWithDepartmentsModel>> GetCoursesWithDepartments();

    Task<Enrollment> UpdateEnrollment(Enrollment enrollment);

    Task<Enrollment> GetEnrollmentById(int studentId, int courseId);

    Task<(string, IEnumerable<string>)> SuggestCoursesDependOnDepartments(int studentId);

    Task<Student> DeleteStudent(int studentId);

    Task<Student> GetStudentById(int studentId);

    Task<IQueryable<string>> GetEnrolledCoursesFor(int studentId);

    Task<IEnumerable<Enrollment>> GetEnrollmentsBy(int studentId);

    Task<int?> GetEnrollmentsCount(int studentId);

    IEnumerable<DepartmentsWithCourses> GetDepartmentsWithCourses();
}
