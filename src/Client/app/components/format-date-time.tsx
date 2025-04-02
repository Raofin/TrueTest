'use client';


export default function FormattedDate({ date }: { date: string }) {


  const getdate = new Date(date);
  const formattedDate =  new Intl.DateTimeFormat('en-US', {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
    hour: 'numeric',
    minute: 'numeric',
    hour12: true,
  });
  return <p>{formattedDate.format(getdate)}</p>;
}
