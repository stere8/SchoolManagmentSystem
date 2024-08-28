import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import { Card, Container, Table } from 'react-bootstrap';
import { BASE_URL } from '../settings';

const ClassDetails = () => {
    const { id } = useParams();
    const [classDetails, setClassDetails] = useState(null);

    useEffect(() => {
        const fetchClassDetails = async () => {
            try {
                const response = await axios.get(`${BASE_URL}/classes/${id}`);
                setClassDetails(response.data);
            } catch (error) {
                console.error('There was an error fetching the class details!', error);
            }
        };

        fetchClassDetails();
    }, [id]);

    if (!classDetails) {
        return <div>Loading...</div>;
    }

    const timeSlots = [
        { start: '08:00', end: '09:00' },
        { start: '09:00', end: '10:00' },
        { start: '10:30', end: '11:30' },
        { start: '11:30', end: '12:30' },
        { start: '13:30', end: '14:30' },
        { start: '14:30', end: '15:30' },
    ];

    const days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];

    const generateTimetable = () => {
        const timetable = {};

        days.forEach(day => {
            timetable[day] = {};
            timeSlots.forEach(slot => {
                const entry = classDetails.classTimetable.find(
                    item => item.dayOfWeek === day && item.startTime === slot.start + ':00' && item.endTime === slot.end + ':00'
                );
                timetable[day][slot.start] = entry ? entry.lessonName : 'N/A';
            });
        });

        return timetable;
    };

    const timetable = generateTimetable();

    return (
        <Container>
            <Card className="mb-3">
                <Card.Header>
                    {classDetails.viewedClass.name} (Grade: {classDetails.viewedClass.gradeLevel}, Year: {classDetails.viewedClass.year})
                </Card.Header>
                <Card.Body>
                    <Card.Title>Teachers</Card.Title>
                    <ul>
                        {classDetails.classTeachers.length > 0 ? (
                            classDetails.classTeachers.map(teacher => (
                                <li key={teacher.staffId}>
                                    {teacher.firstName} {teacher.lastName} ({teacher.subjectExpertise})
                                </li>
                            ))
                        ) : (
                            <li>N/A</li>
                        )}
                    </ul>
                    <Card.Title className="mt-3">Students</Card.Title>
                    <ul>
                        {classDetails.classStudents.length > 0 ? (
                            classDetails.classStudents.map(student => (
                                <li key={student.studentId}>
                                    {student.firstName} {student.lastName}
                                </li>
                            ))
                        ) : (
                            <li>N/A</li>
                        )}
                    </ul>
                    <Card.Title className="mt-3">Timetable</Card.Title>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>Time</th>
                                {days.map(day => (
                                    <th key={day}>{day}</th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>
                            {timeSlots.map(slot => (
                                <tr key={slot.start}>
                                    <td>{slot.start} - {slot.end}</td>
                                    {days.map(day => (
                                        <td key={day}>{timetable[day][slot.start]}</td>
                                    ))}
                                </tr>
                            ))}
                        </tbody>
                    </Table>
                </Card.Body>
            </Card>
        </Container>
    );
};

export default ClassDetails;
