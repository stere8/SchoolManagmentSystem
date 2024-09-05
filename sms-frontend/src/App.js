import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
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

                {/* Unprotected Routes */}
                <Route path="/attendance" element={<Attendance />} />
                <Route path="/attendance/add" element={<AddEditAttendance />} />
                <Route path="/attendance/edit/:id" element={<AddEditAttendance />} />

                {/* Classes Routes */}
                <Route path="/classes" element={<Classes />} />
                <Route path="/classes/:id" element={<ClassDetails />} />

                {/* Enrollment Routes */}
                <Route path="/enrollments" element={<Enrollments />} />
                <Route path="/enrollments/add" element={<AddEditEnrollment />} />
                <Route path="/enrollments/edit/:id" element={<AddEditEnrollment />} />

                {/* Teacher Enrollment Routes */}
                <Route path="/teacher-enrollments" element={<TeacherEnrollments />} />
                <Route path="/teacher-enrollments/add" element={<AddEditTeacherEnrollment />} />
                <Route path="/teacher-enrollments/edit/:id" element={<AddEditTeacherEnrollment />} />

                {/* Lessons Routes */}
                <Route path="/lessons" element={<Lessons />} />
                <Route path="/lessons/add" element={<AddEditLesson />} />
                <Route path="/lessons/edit/:id" element={<AddEditLesson />} />

                {/* Marks Routes */}
                <Route path="/marks" element={<Marks />} />
                <Route path="/marks/add" element={<AddEditMark />} />
                <Route path="/marks/edit/:id" element={<AddEditMark />} />

                {/* Staff Routes */}
                <Route path="/staff" element={<Staff />} />
                <Route path="/staff/add" element={<AddEditStaff />} />
                <Route path="/staff/edit/:id" element={<AddEditStaff />} />

                {/* Students Routes */}
                <Route path="/students" element={<Students />} />
                <Route path="/students/add" element={<AddEditStudent />} />
                <Route path="/students/edit/:id" element={<AddEditStudent />} />

                {/* Timetable Route */}
                <Route path="/timetable" element={<Timetable />} />

                {/* Default Route */}
                <Route path="/" element={isAuthenticated() ? <Navigate to="/classes" /> : <Navigate to="/login" />} />
            </Routes>
        </Router>
    );
}

export default App;