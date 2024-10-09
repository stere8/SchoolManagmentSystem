import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const TeacherMarks = () => {
    const [marks, setMarks] = useState([]);
    const [students, setStudents] = useState([]);
    const [lessons, setLessons] = useState({});
    const [teacherDetails, setTeacherDetails] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const userDetails = JSON.parse(localStorage.getItem('details'));

                if (userDetails) {
                    setTeacherDetails(userDetails);

                    const [marksResponse] = await Promise.all([
                        axios.get(`${BASE_URL}/marks`)
                    ]);

                    // Fetch students based on each assigned class
                    let studentsList = [];
                    await Promise.all(
                        userDetails.assignedClasses.map(async assignedClass => {
                            const studentsResponse = await axios.get(`${BASE_URL}/students/class/${assignedClass.classId}`);
                            studentsList = [...studentsList, ...studentsResponse.data];
                        })
                    );

                    // Remove duplicate students
                    const uniqueStudents = studentsList.filter(
                        (student, index, self) => index === self.findIndex(s => s.studentId === student.studentId)
                    );

                    // Filter marks to include only those related to the teacher's classes and lessons
                    const filteredMarks = marksResponse.data.filter(mark =>
                        userDetails.assignedClasses.some(
                            assignedClass => assignedClass.classId === mark.classId && assignedClass.lessonId === mark.lessonId
                        )
                    );

                    console.log('Filtered Students:', uniqueStudents);
                    console.log('Filtered Marks:', filteredMarks);

                    setMarks(filteredMarks);
                    setStudents(uniqueStudents);

                    // Fetch detailed lesson information for assigned classes
                    const lessonData = {};
                    await Promise.all(
                        userDetails.assignedClasses.map(async assignedClass => {
                            if (assignedClass.lessonId) {
                                try {
                                    const lessonResponse = await axios.get(`${BASE_URL}/lessons/${assignedClass.lessonId}`);
                                    lessonData[assignedClass.lessonId] = lessonResponse.data;
                                } catch (lessonError) {
                                    console.error(`Error fetching lesson with ID ${assignedClass.lessonId}:`, lessonError);
                                }
                            }
                        })
                    );

                    setLessons(lessonData);
                }
            } catch (error) {
                console.error('There was an error fetching the data!', error);
            }
        };

        fetchData();
    }, []);

    const deleteMark = id => {
        axios.delete(`${BASE_URL}/marks/${id}`)
            .then(() => setMarks(marks.filter(mark => mark.markId !== id)))
            .catch(error => console.error('Error deleting mark:', error));
    };

    const findStudentName = studentId => {
        const student = students.find(s => s.studentId === studentId);
        return student ? `${student.firstName} ${student.lastName}` : 'Unknown Student';
    };

    const findLessonName = lessonId => {
        const lesson = lessons[lessonId];
        return lesson ? lesson.name : 'Unknown Lesson';
    };

    return (
        <div>
            <h1>Your Students' Marks</h1>
            {teacherDetails && teacherDetails.assignedClasses.length > 0 && (
                <Button
                    as={Link}
                    to={`/teachermarks/add?lessonId=${teacherDetails.assignedClasses[0].lessonId}`}
                    variant="primary"
                >
                    Add Mark
                </Button>
            )}
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Student</th>
                        <th>Lesson</th>
                        <th>Mark</th>
                        <th>Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {marks.map(mark => (
                        <tr key={mark.markId}>
                            <td>{findStudentName(mark.studentId)}</td>
                            <td>{findLessonName(mark.lessonId)}</td>
                            <td>{mark.markValue}</td>
                            <td>{new Date(mark.date).toLocaleDateString()}</td>
                            <td>
                                <Button as={Link} to={`/marks/edit/${mark.markId}`} variant="warning">Edit</Button>
                                <Button onClick={() => deleteMark(mark.markId)} variant="danger">Delete</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default TeacherMarks;
