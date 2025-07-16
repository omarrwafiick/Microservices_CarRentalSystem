import { Badge, Drawer, Image, List, Space, Typography } from 'antd' 
import React, { useEffect, useState } from 'react'
import logo from '../../../../public/assets/images/logo2.png'
import { BellFilled } from '@ant-design/icons'
import { useLocation } from 'react-router-dom'

const AppHeader = () => { 
  const [commentsState, setCommentsState] = useState(false);
  const [notifications, setNotifications] = useState([]);
  const [comments, setComments] = useState([]);
  const location = useLocation().pathname;

  useEffect(()=>{ 
    setComments(['dsdsdsds','weewewewewew','weeeeeeeeewe','weeeeeeeeee','dsdsdsds','weewewewewew','weeeeeeeeewe','weeeeeeeeee']);
  },
  []);

  return (
    <header className='h-20 sticky top-0 bg-white z-40 w-full flex justify-between items-center header'>
      
      <Space> 
        <Image width={180} src={logo}></Image> 
        <h1 className='text-4xl ms-16 w-full text-start font-semibold'>
          {
            location == "/" ? "Dashboard" :""
          }
          
        </h1>
      </Space>

      <Space> 
        <Badge count={comments.length}> 
          <BellFilled onClick={() => setCommentsState(true)} className='text-2xl' />
        </Badge> 
      </Space>
      <Drawer title='comments' open={commentsState} onClose={() => setCommentsState(false)} maskClosable={true}>
        <List 
          dataSource={comments}
          renderItem={(item)=>{
            return (
              <List.Item>
                <Typography.Text>{item}</Typography.Text>
              </List.Item>
            )
          }}
        ></List>
      </Drawer> 
    </header>
  )
}

export default AppHeader