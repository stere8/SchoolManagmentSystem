import React from 'react';
import { Navigate } from 'react-router-dom';
import authService from '../services/authService';

const ProtectedRoute = ({ children, roles }) => {
    const user = authService.getCurrentUser();

    if (!user) {
        return <Navigate to="/login" />;
    }

    if (roles && roles.indexOf(user.role) === -1) {
        return <Navigate to="/" />;
    }

    return children;
};

export default ProtectedRoute;