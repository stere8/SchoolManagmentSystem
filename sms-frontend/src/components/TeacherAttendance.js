import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { Table, Button, Alert } from 'react-bootstrap';
import { BASE_URL } from '../settings';

const TeacherAttendance = ({ classId }) => {
    const [attendance, setAttendance] = useState([]);
    const [students, setStudents] = useState([]);
    const [lessons, setLessons] = useState([]);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchAttendanceData = async () => {
            try {
                const userDetails = JSON.parse(localStorage.getItem('details'));

                if (userDetails && userDetails.assignedClasses.length > 0) {
                    // Fetch students who are in the teacher's assigned classes
                    const classId = userDetails.assignedClasses[0].classId;
                    const studentsResponse = await axios.get(`${BASE_URL}/students/class/${classId}`);
                    setStudents(studentsResponse.data);

                    // Fetch attendance records
                    const lessonId = userDetails.assignedClasses[0].lessonId;
                    const attendanceByLesson = await axios.get(`${BASE_URL}/attendance/${lessonId}`);
                    const filteredAttendance = attendanceByLesson.data.filter(record =>
                        studentsResponse.data.some(student => student.studentId === record.studentId)
                    );
                    setAttendance(filteredAttendance);

                    // Fetch lesson data
                    const lessonResponse = await axios.get(`${BASE_URL}/lessons/${lessonId}`);
                    setLessons([lessonResponse.data]);
                }
            } catch (error) {
                console.error('There was an error fetching the data!', error);
                setError('Error fetching data. Please try again later.');
            }
        };

        if (classId) {
            fetchAttendanceData();
        }
    }, [classId]);

    const deleteAttendance = (id) => {
        axios.delete(`${BASE_URL}/attendance/${id}`)
            .then(() => setAttendance(attendance.filter(record => record.attendanceId !== id)))
            .catch(error => {
                console.error('Error deleting attendance record:', error);
                setError('Error deleting attendance record. Please try again.');
            });
    };

    const getStudentNameById = (id) => {
        const student = students.find(student => student.studentId === id);
        return student ? `${student.firstName} ${student.lastName}` : 'Unknown Student';
    };

    const getLessonNameById = (id) => {
        const lesson = lessons.find(lesson => lesson.lessonId === id);
        return lesson ? lesson.name : 'Unknown Lesson';
    };

    const formatDate = (dateString) => {
        return dateString.split('T')[0]; // Removes the 'T00:00:00' part
    };

    return (
        <div>
            <h1>Attendance Records</h1>
            {error && <Alert variant="danger">{error}</Alert>}
            <Button as={Link} to="/attendance/add" variant="primary" className="mb-3">Add Attendance Record</Button>
            {attendance.length > 0 ? (
                <Table striped bordered hover responsive>
                    <thead>
                        <tr>
                            <th>Student</th>
                            <th>Lesson</th>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {attendance.map(record => (
                            <tr key={record.attendanceId}>
                                <td>{getStudentNameById(record.studentId)}</td>
                                <td>{getLessonNameById(record.lessonId)}</td>
                                <td>{formatDate(record.date)}</td>
                                <td>{record.status}</td>
                                <td>
                                    <Button as={Link} to={`/attendance/edit/${record.attendanceId}`} variant="warning" className="mr-2">Edit</Button>
                                    <Button onClick={() => deleteAttendance(record.attendanceId)} variant="danger">Delete</Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            ) : (
                <p>No attendance records available for the selected students.</p>
            )}
        </div>
    );
};

export default TeacherAttendance;
