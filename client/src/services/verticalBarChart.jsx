import React from 'react'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Bar } from 'react-chartjs-2'; 

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

export const options = (position, title) =>{
    return {
        responsive: true,
        plugins: {
            legend: {
            position: position,
            },
            title: {
            display: true,
            text: title,
            },
        },
    };
} 
 

export const buildChartData = (data = {}, labels) => {
  return {
    labels,
    datasets: [
      {
        label: 'Dataset 1',
        data: labels.map(() => Math.random() * 1000),
        backgroundColor: 'rgba(255, 145, 90, 0.31)',
      },
      {
        label: 'Dataset 2',
        data: labels.map(() => Math.random() * 1000),
        backgroundColor: 'rgba(255, 145, 90, 0.937)',
      },
    ],
  };
};

export const VerticalBarChart = ({style, position, title, data, labels }) => {
  return <Bar style={style} options={options(position, title)} data={buildChartData(data, labels)} />;
};