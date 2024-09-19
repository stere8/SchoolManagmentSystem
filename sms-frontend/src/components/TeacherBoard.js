import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { BASE_URL } from '../settings';
import authService from '../services/authService';
import { Table, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const TeacherBoard = () => {
    const [teacherData, setTeacherData] = useState({
        enrollments: [],
        attendance: [],
        marks: []
    });

    useEffect(() => {
        const fetchTeacherData = async () => {
            const currentUser = authService.getCurrentUser();
            if (currentUser && currentUser.role === 'teacher') {
                try {
                    const [enrollmentsResponse, attendanceResponse, marksResponse] = await Promise.all([
                        axios.get(`${BASE_URL}/TeacherEnrollments/teacher/${currentUser.id}`),
                        axios.get(`${BASE_URL}/attendance/teacher/${currentUser.id}`),
                        axios.get(`${BASE_URL}/marks/teacher/${currentUser.id}`)
                    ]);

                    setTeacherData({
                        enrollments: enrollmentsResponse.data,
                        attendance: attendanceResponse.data,
                        marks: marksResponse.data
                    });
                } catch (error) {
                    console.error('Error fetching teacher data:', error);
                }
            }
        };

        fetchTeacherData();
    }, []);

    const getStudentNameById = (id, students) => {
        const student = students.find(student => student.studentId === id);
        return student ? `${student.firstName} ${student.lastName}` : 'Unknown Student';
    };

    const getLessonNameById = (id, lessons) => {
        const lesson = lessons.find(lesson => lesson.lessonId === id);
        return lesson ? lesson.name : 'Unknown Lesson';
    };

    return (
        <div>
            <h1>Teacher Board</h1>
            <p>Welcome to the Teacher Board. Here you can manage your classes, input marks, and add attendance for your students.</p>

            <h2>Your Enrollments</h2>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Class</th>
                        <th>Teacher</th>
                        <th>Lesson</th>
                    </tr>
                </thead>
                <tbody>
                    {teacherData.enrollments.map(enrollment => (
                        <tr key={enrollment.enrollmentRef}>
                            <td>{enrollment.assignedClass || 'N/A'}</td>
                            <td>{enrollment.enrolledTeacher || 'N/A'}</td>
                            <td>{enrollment.assignedLesson || 'N/A'}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <h2>Recent Attendance</h2>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Student</th>
                        <th>Lesson</th>
                        <th>Date</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {teacherData.attendance.map(record => (
                        <tr key={record.attendanceId}>
                            <td>{getStudentNameById(record.studentId, teacherData.students)}</td>
                            <td>{getLessonNameById(record.lessonId, teacherData.lessons)}</td>
                            <td>{new Date(record.date).toLocaleDateString()}</td>
                            <td>{record.status}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <h2>Recent Marks</h2>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Student</th>
                        <th>Lesson</th>
                        <th>Mark</th>
                        <th>Date</th>
                    </tr>
                </thead>
                <tbody>
                    {teacherData.marks.map(mark => (
                        <tr key={mark.markId}>
                            <td>{getStudentNameById(mark.studentId, teacherData.students)}</td>
                            <td>{getLessonNameById(mark.lessonId, teacherData.lessons)}</td>
                            <td>{mark.markValue}</td>
                            <td>{new Date(mark.date).toLocaleDateString()}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>

            <Button as={Link} to="/attendance/add" variant="primary">Add Attendance</Button>
            <Button as={Link} to="/marks/add" variant="primary">Add Mark</Button>
        </div>
    );
};

export default TeacherBoard;