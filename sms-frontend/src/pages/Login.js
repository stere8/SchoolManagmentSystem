import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import authService from '../services/authService';
import '../Styles/Login.css'; // Ensure this path matches your project structure

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

   const handleLogin = async (e) => {
    e.preventDefault();
    try {
        const response = await authService.login(email, password);
        const { token, role, details } = response; // Fixed: Changed "data" to "response"

        localStorage.setItem('details', JSON.stringify(details));

        switch (role.toLowerCase()) {
            case 'student':
                navigate('/student-dashboard');
                break;
            case 'teacher':
                navigate('/teacher-board');
                break;
            case 'parent':
                navigate('/parent-dashboard');
                break;
            case 'admin':
                navigate('/admin-dashboard');
                break;
            default:
                navigate('/classes'); // Default redirection
                break;
        }
    } catch (error) {
        console.error('Login failed', error);
    }
};


    return (
        <div className="login-container">
            <h2>Login</h2>
            <form onSubmit={handleLogin} className="login-form">
                <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        id="email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Enter your email"
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Password</label>
                    <input
                        id="password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Enter your password"
                        required
                    />
                </div>
                <button type="submit" className="login-button">Login</button>
            </form>
            <p className="register-link">
                Don't have an account? <Link to="/register">Register here</Link>
            </p>
        </div>
    );
};

export default Login;
