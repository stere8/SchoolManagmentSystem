import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, Container, Table } from 'react-bootstrap';
import { BASE_URL } from '../settings';

const TeacherDashboard = () => {
    const [userDetails, setUserDetails] = useState(null);
    const [classesWithDetails, setClassesWithDetails] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const user = JSON.parse(localStorage.getItem('details'));
                setUserDetails(user);

                if (user && user.assignedClasses) {
                    const classPromises = user.assignedClasses.map(async (classItem) => {
                        const classResponse = await axios.get(`${BASE_URL}/classes/${classItem.classId}`);
                        return {
                            ...classItem,
                            classDetails: classResponse.data
                        };
                    });

                    const classData = await Promise.all(classPromises);
                    setClassesWithDetails(classData);
                }
            } catch (error) {
                console.error('Error fetching teacher or class details:', error);
            }
        };

        fetchData();
    }, []);

    if (!userDetails) {
        return <p>Loading...</p>;
    }

    return (
        <Container>
            <h2>Welcome, {userDetails.firstName} {userDetails.lastName}</h2>
            <h3>Your Assigned Classes:</h3>
            {classesWithDetails.map((classItem) => (
                <Card key={classItem.classId} className="mb-3">
                    <Card.Header>
                        Class: {classItem.classDetails.viewedClass.name} (Grade: {classItem.classDetails.viewedClass.gradeLevel}, Year: {classItem.classDetails.viewedClass.year})
                    </Card.Header>
                    <Card.Body>
                        <h4>Class Teachers:</h4>
                        <ul>
                            {classItem.classDetails.classTeachers.length > 0 ? (
                                classItem.classDetails.classTeachers.map(teacher => (
                                    <li key={teacher.staffId}>
                                        {teacher.firstName} {teacher.lastName} ({teacher.subjectExpertise})
                                    </li>
                                ))
                            ) : (
                                <li>N/A</li>
                            )}
                        </ul>

                        <h4>Enrolled Students:</h4>
                        {classItem.classDetails.classStudents && classItem.classDetails.classStudents.length > 0 ? (
                            <Table striped bordered hover>
                                <thead>
                                    <tr>
                                        <th>Student ID</th>
                                        <th>First Name</th>
                                        <th>Last Name</th>
                                        <th>Grade Level</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {classItem.classDetails.classStudents.map(student => (
                                        <tr key={student.studentId}>
                                            <td>{student.studentId}</td>
                                            <td>{student.firstName}</td>
                                            <td>{student.lastName}</td>
                                            <td>{student.gradeLevel}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </Table>
                        ) : (
                            <p>No students enrolled in this class.</p>
                        )}

                        <h4 className="mt-3">Class Timetable:</h4>
                        <Table striped bordered hover>
                            <thead>
                                <tr>
                                    <th>Time</th>
                                    {['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'].map(day => (
                                        <th key={day}>{day}</th>
                                    ))}
                                </tr>
                            </thead>
                            <tbody>
                                {[
                                    { start: '08:00', end: '09:00' },
                                    { start: '09:00', end: '10:00' },
                                    { start: '10:30', end: '11:30' },
                                    { start: '11:30', end: '12:30' },
                                    { start: '13:30', end: '14:30' },
                                    { start: '14:30', end: '15:30' }
                                ].map(slot => (
                                    <tr key={slot.start}>
                                        <td>{slot.start} - {slot.end}</td>
                                        {['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'].map(day => {
                                            const entry = classItem.classDetails.classTimetable.find(
                                                item => item.dayOfWeek === day && item.startTime === `${slot.start}:00` && item.endTime === `${slot.end}:00`
                                            );
                                            return <td key={day}>{entry ? entry.lessonName : 'N/A'}</td>;
                                        })}
                                    </tr>
                                ))}
                            </tbody>
                        </Table>
                    </Card.Body>
                </Card>
            ))}
        </Container>
    );
};

export default TeacherDashboard;
