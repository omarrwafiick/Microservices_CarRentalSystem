import { DollarOutlined, ShoppingCartOutlined, ShoppingOutlined, UserOutlined } from '@ant-design/icons'
import { Card, Space, Statistic, Table, Typography, Flex, Progress } from 'antd'
import React, { useEffect, useState } from 'react'
import { getOrders } from '../../services/fetchData';
import { VerticalBarChart } from '../../services/verticalBarChart';
import { PolarAreaChart } from '../../services/polarAreaChart';
import { PieChart } from '../../services/pieChart';

const Dashboard = () => {
  const [orders,setOrders] = useState();
  const [loading,setLoading] = useState(false); 
  
  useEffect(()=>{
    setLoading(true);
    const getData = async () =>{
      await getOrders().then(res =>{
          const data = res.products;
          setOrders(data);
          setLoading(false);
        }
      ); 
    }
    getData();
  },[])
  
  return (
    <div className='w-full' > 
      <div className="w-full flex flex-col"> 
        <div className='w-full flex justify-between mt-3 mb-3'>
            <DashboardCard title={'Total Income'} value={'$25,024.32'} date={'Saturday, 30 Sep 2025'} index={'59%'} sub={'$4,593.23'} up={true} />
            <DashboardCard title={'Total Outcome'} value={'$12,456.12'} date={'Saturday, 30 Sep 2025'} index={'12%'} sub={'$4,593.23'} up={false} />
            <DashboardCard title={'Total Profit'} value={'$32,245.45'} date={'Saturday, 30 Sep 2025'} index={'4%'} sub={'$4,593.23'} up={true} />
        </div> 
        <Space style={{ display: 'flex', justifyContent: 'space-between', width: '100%', marginTop: '12px', marginBottom: '12px'}}> 
            <VerticalBarChart
              style={{width: 900, height: 150 , borderColor: 'rgba(0, 0, 0, 0.1)', borderWidth: '2px', borderRadius: '0.5rem' }}
              data={""} 
              title={'General Income'} 
              position={'bottom'} 
              labels={['January', 'February', 'March', 'April', 'May', 'June', 'July']} />
            {/* <PolarAreaChart style={{width: 500, height: 350}} />
            <PieChart style={{width: 500, height: 350}} /> */}
            <div className='flex w-[440px] '>
              <div className='w-full border-2 border-black/10 rounded-lg p-6'>
                <h3 className='capitalize font-semibold text-md'>expense records</h3>
                <h4 className='opacity-50 text-sm mt-2'>Saturday, 30 Sep 2025</h4>
                <div>
                  <ExpenseCard title={'pay monthly electricity bill'} sub={'monthly bills'} value={'-$232.23'} />
                  <ExpenseCard title={'pay monthly car bill'} sub={'monthly bills'} value={'-$232.23'} />
                  <ExpenseCard title={'pay monthly school bill'} sub={'monthly bills'} value={'-$232.23'} />
                </div>
              </div>
            </div> 
        </Space>   
        <Space style={{ width: '100%' , marginTop: '12px', marginBottom: '12px'}}>
            <OrdersTable data={orders} loading={loading} />
        </Space>
      </div>
    </div>
  )
}

function DashboardCard({title, value, index, date, sub, up}){
  return (
      <div className='w-[440px] border-2 border-black/10 rounded-lg p-6'> 
        <h1 className='mb-12 font-semibold capitalize'>{title}</h1>
        <div className='flex flex-col'> 
          <div className='flex w-full mb-4'>
            <div className='flex w-9/12'>
              <h3 className='text-4xl font-semibold'>{value}</h3>
            </div>
            <div className='flex justify-end items-center w-3/12'>
              <h5 className={`${up ? 'bg-green-300/25 text-green-700': 'bg-red-300/25 text-red-700'} text-sm font-semibold rounded-2xl p-2 text-center w-14`}>{index}</h5>
            </div>
          </div>
          <div className='flex w-full mb-4'>
            <div className='flex w-9/12'>
              <h4 className='opacity-50'>{date}</h4>
            </div>
            <div className='flex justify-end w-3/12'>
               <h4 className='font-semibold'>{sub}</h4>
            </div>
          </div> 
        </div>
      </div>
  )
}

function ExpenseCard({title, value, sub}){
  return (
      <div className='flex w-full border-2 border-black/5 rounded-lg p-6 mt-2 mb-2 overflow-hidden' > 
        <div className='flex flex-col justify-center items-start w-9/12'> 
          <h3 className='capitalize font-semibold'>{title}</h3> 
          <h3 className='capitalize mt-1 opacity-50'>{sub}</h3>
        </div>
        <div className='flex justify-center items-center w-3/12'>
            <h3 className='font-semibold'>{value}</h3>
        </div>
      </div>
  )
} 

function OrdersTable({data, loading}){
  return (
    <>
      <h2 className='capitalize text-2xl font-semibold mb-3'>transaction history</h2>
      <Table 
      style={{borderColor: 'rgba(0, 0, 0, 0.1)', borderWidth: '2px', borderRadius: '0.5rem' }}
      columns={[
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        },
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        }
        ,
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        }
        ,
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        },
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        },
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        },
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        }
      ]}
      dataSource={data}
      loading={loading}
      pagination={{
        pageSize: 10
      }}
      >  
      </Table>
    </>
  )
} 

export default Dashboard
