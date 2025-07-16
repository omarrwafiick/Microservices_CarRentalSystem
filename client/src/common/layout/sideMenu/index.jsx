import React from 'react'
import { AppstoreOutlined, SwitcherOutlined, DollarOutlined, WalletOutlined, 
        NotificationOutlined,ShoppingOutlined, LikeOutlined, MessageOutlined,
        LogoutOutlined   } from '@ant-design/icons'
import { Menu, Typography } from 'antd'
import { useNavigate } from 'react-router-dom'
 
const AppSideMenu = () => {
  const navigate = useNavigate();
  return (
    <div className='sticky top-0 bg-white min-h-screen w-2/12 flex flex-col justify-start items-center border-r-2 border-black/10'>  
        <Menu   
            className="w-7/12 custom-menu"
            onClick={(item)=>{ 
                navigate(item.key);
            }}
            items={
            [
                {
                    label:'Dashboard',
                    icon: <AppstoreOutlined />, 
                    key:'/'
                },
                {
                    label:'Category',
                    icon: <ShoppingOutlined />,
                    key:'/category'
                },
                {
                    label:'Wallet',
                    icon: <WalletOutlined />,
                    key:'/wallet'
                },
                {
                    label:'Notification',
                    icon:<NotificationOutlined />,
                    key:'/notification'
                },
                {
                    label:'Transaction',
                    icon:<SwitcherOutlined   tlined />,
                    key:'/transaction'
                }
                ,
                {
                    label:'Trade',
                    icon:<DollarOutlined  />,
                    key:'/trade'
                }
                ,
                {
                    label:'Support',
                    icon:<LikeOutlined />,
                    key:'/support'
                }
                ,
                {
                    label:'FAQ',
                    icon:<MessageOutlined />,
                    key:'/faq'
                }
                ,
                {
                    label:'Logout',
                    icon:<LogoutOutlined />
                }
            ]
        }> 
        </Menu> 
    </div>
  )
}

export default AppSideMenu