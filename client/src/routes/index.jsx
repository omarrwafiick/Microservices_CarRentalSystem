import React from 'react'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import App from '../App';  
import Dashboard from '../features/dashboard/index';  
import Analytics from '../features/analytics/index'; 
import Booking from '../features/booking/index'; 
import CurrentPickup from '../features/currentPickup/index'; 
import Maintenance from '../features/maintenance/index'; 
import Recommendation from '../features/recommendation/index'; 
import Vehicle from '../features/vehicle/index'; 
import Transaction from '../features/transaction/index';  
import Signup from '../features/authentication/signup/index'; 
import Login from '../features/authentication/login/index'; 
import Resetpassword from '../features/authentication/resetpassword/index'; 
import Forgetpassword from '../features/authentication/forgetpassword/index'; 
import NotFound from '../common/layout/notFound/index'; 

function AppRouter() {
  return ( 
    <BrowserRouter>
      <Routes> 
        <Route path="/" element={<App />}>
          <Route index element={<Dashboard />} /> 
          <Route path='analytics' element={<Analytics />} /> 
          <Route path='booking' element={<Booking />} /> 
          <Route path='currentPickup' element={<CurrentPickup />} /> 
          <Route path='maintenance' element={<Maintenance />} /> 
          <Route path='recommendation' element={<Recommendation />} /> 
          <Route path='vehicle' element={<Vehicle />} /> 
          <Route path='transaction' element={<Transaction />} />  
          <Route path='signup' element={<Signup />} /> 
          <Route path='login' element={<Login />} /> 
          <Route path='resetpassword' element={<Resetpassword />} /> 
          <Route path='forgetpassword' element={<Forgetpassword />} /> 
          <Route path='*' element={<NotFound />} /> 
        </Route>
      </Routes>
    </BrowserRouter>
  ); 
}

export default AppRouter;
