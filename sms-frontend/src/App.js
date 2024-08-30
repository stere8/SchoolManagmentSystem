// Path: /sms-frontend/src/App.js

import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import axios from 'axios';
import authService from './services/authService';
import Navbar from './components/NavigationBar';
import Attendance from './components/Attendance';
import Classes from './components/Classes';
import ClassDetails from './components/ClassDetails';
import Enrollments from './components/Enrollments';
import Lessons from './components/Lessons';
import Marks from './components/Marks';
import Staff from './components/Staff';
import Students from './components/Students';
import Timetable from './components/Timetable';
import AddEditStudent from './components/AddEditStudent';
import AddEditStaff from './components/AddEditStaff';
import AddEditEnrollment from './components/AddEditEnrollment';
import AddEditTeacherEnrollment from './components/AddEditTeacherEnrollment';
import TeacherEnrollments from './components/TeacherEnrollments';
import AddEditLesson from './components/AddEditLesson';
import AddEditAttendance from './components/AddEditAttendance';
import AddEditMark from './components/AddEditMark';
import Login from './components/Login';
import Register from './components/Register';
import ProtectedRoute from './components/ProtectedRoute';

// Axios interceptor to add JWT token to Authorization header
axios.interceptors.request.use(
    (config) => {
        const user = authService.getCurrentUser();
        if (user && user.token) {
            config.headers['Authorization'] = 'Bearer ' + user.token;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

function App() {
    const isAuthenticated = () => {
        return !!authService.getCurrentUser();
    };

    return (
        <Router>
            <Navbar />
            <Routes>
                {/* Public Routes */}
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />

                {/* Protected Routes */}
                <Route
                    path="/attendance"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <Attendance />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/attendance/add"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <AddEditAttendance />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/attendance/edit/:id"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <AddEditAttendance />
                        </ProtectedRoute>
                    }
                />

                {/* Classes Routes */}
                <Route
                    path="/classes"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher', 'student']}>
                            <Classes />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/classes/:id"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher', 'student']}>
                            <ClassDetails />
                        </ProtectedRoute>
                    }
                />

                {/* Enrollment Routes */}
                <Route
                    path="/enrollments"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <Enrollments />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/enrollments/add"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditEnrollment />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/enrollments/edit/:id"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditEnrollment />
                        </ProtectedRoute>
                    }
                />

                {/* Teacher Enrollment Routes */}
                <Route
                    path="/teacher-enrollments"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <TeacherEnrollments />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/teacher-enrollments/add"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditTeacherEnrollment />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/teacher-enrollments/edit/:id"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditTeacherEnrollment />
                        </ProtectedRoute>
                    }
                />

                {/* Lessons Routes */}
                <Route
                    path="/lessons"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <Lessons />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/lessons/add"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <AddEditLesson />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/lessons/edit/:id"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <AddEditLesson />
                        </ProtectedRoute>
                    }
                />

                {/* Marks Routes */}
                <Route
                    path="/marks"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <Marks />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/marks/add"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <AddEditMark />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/marks/edit/:id"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <AddEditMark />
                        </ProtectedRoute>
                    }
                />

                {/* Staff Routes */}
                <Route
                    path="/staff"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <Staff />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/staff/add"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditStaff />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/staff/edit/:id"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditStaff />
                        </ProtectedRoute>
                    }
                />

                {/* Students Routes */}
                <Route
                    path="/students"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher']}>
                            <Students />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/students/add"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditStudent />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/students/edit/:id"
                    element={
                        <ProtectedRoute roles={['admin']}>
                            <AddEditStudent />
                        </ProtectedRoute>
                    }
                />

                {/* Timetable Route */}
                <Route
                    path="/timetable"
                    element={
                        <ProtectedRoute roles={['admin', 'teacher', 'student']}>
                            <Timetable />
                        </ProtectedRoute>
                    }
                />

                {/* Default Route */}
                <Route
                    path="/"
                    element={
                        isAuthenticated() ? <Navigate to="/classes" /> : <Navigate to="/login" />
                    }
                />
            </Routes>
        </Router>
    );
}

export default App;