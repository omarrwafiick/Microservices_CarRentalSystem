import React from 'react'
import { createRoot } from 'react-dom/client'  
import AppRouter from './routes/index.jsx';

const root = createRoot(document.getElementById('root'));

root.render(<AppRouter />);