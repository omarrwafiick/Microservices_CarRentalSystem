import { Avatar, Space, Table, Typography } from 'antd'
import React, { useEffect, useState } from 'react'
import { getOrders } from '../../services/fetchData';

const Transaction = () => {
  const [data,setData] = useState();
  const [loading,setLoading] = useState(false);
  useEffect(()=>{
    setLoading(true);
    const getData = async () =>{
      await getOrders().then(res =>{
          const data = res.products;
          setData(data.splice(0,10));
          setLoading(false);
        }
      ); 
    }
    getData();
  },[]);

  return (
    <Space size={20} direction='vertical'> 
      <Space>
        <TradeTable data={data} loading={loading} />
      </Space>
    </Space>
  )
} 

function TradeTable({data, loading}){
  return (
    <>
      <Typography.Text>Trade</Typography.Text>
      <Table
      columns={[
        {
          title: 'Thumbnail',
          dataIndex: 'thumbnail',
          render:(value)=> <Avatar src={value} /> 
        }
        ,
        {
          title: 'Title',
          dataIndex: 'title'
        },
        {
          title: 'Quantity',
          dataIndex: 'quantity'
        },
        {
          title: 'Price',
          dataIndex: 'price',
          render:(value)=><span>{'$'+value}</span>
        }
      ]}
      dataSource={data}
      loading={loading}
      pagination={false}
      >  
      </Table>
    </>
  )
} 

export default Transaction
