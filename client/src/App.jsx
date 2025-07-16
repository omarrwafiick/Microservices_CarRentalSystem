import React from 'react'
import './App.css'
import AppHeader from './common/layout/header'  
import AppSideMenu from './common/layout/sideMenu' 
import { Space } from 'antd'
import { Outlet } from 'react-router-dom'

function App() { 

  return (
   <div className="min-h-screen flex flex-col w-full justify-between items-center">
      <AppHeader /> 
      <div className='w-full flex'>
        <AppSideMenu /> 
        <div className="w-full flex pe-12 ps-12"> 
          <Outlet />
        </div>
      </div> 
    </div>
  )
}

export default App
