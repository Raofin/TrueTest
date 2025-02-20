"use client";
import dynamic from 'next/dynamic';
import 'chart.js/auto';
const Line = dynamic(() => import('react-chartjs-2').then((mod) => mod.Line), {
    ssr: false,
});
const Pie = dynamic(() => import('react-chartjs-2').then((mod) => mod.Pie), {
    ssr: false,
});
const data = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'],
    datasets: [
        {
            label: 'Line Chart',
            data: [10,5,30,20,50,60],
            fill: false,
            borderColor: 'rgb(75, 192, 192)',
            tension: 0.1,
        },
    ],
};
const piedata = {
    labels: ['Pass', 'Fail'],
    datasets: [{
        label: 'result',
        data: [100, 300],
        backgroundColor: ['rgb(38,123,14)','rgb(255, 99, 132)'],
        hoverOffset: 4
    }]
};
const Piechart = () => {
    return (
     <div className="flex mt-24 space-x-16 ml-16">
         <div style={{ width: '700px', height: '500px' }}>
             <h1>Attend Exam </h1>
             <Line data={data} />
         </div>
         <div style={{ width: '300px', height: '300px' }}>
             <h1>Exam Result</h1>
             <Pie data={piedata} />
         </div>
     </div>
    );
};
export default Piechart;
