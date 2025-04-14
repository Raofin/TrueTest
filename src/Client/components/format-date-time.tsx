
type FormatTimeProps = {
  minute: number;
};

export function FormattedDateWeekday({ date }: {readonly date: string }) {
  const openDate = new Date(date);

  const formattedDate = openDate.toLocaleDateString(undefined, {
    weekday: 'long',
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  });
  return formattedDate
}
export function FormattedDateYear({ date }: {readonly date: string }) {
  const openDate = new Date(date);
  return openDate.toLocaleDateString('en-US', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit'
  });
}

export function FormattedTime({ date }: {readonly date: string }) { 

  const openDate = new Date(date);
  const formattedTime = openDate.toLocaleTimeString(undefined, {
    hour: '2-digit',
    minute: '2-digit',
    hour12: true
  });
  
  return formattedTime;
}


export function  FormatTimeHourMinutes({minute}:FormatTimeProps) {
  const hours = Math.floor(minute / 60)
  const minutes =Math.floor(minute % 60).toString().padStart(2, '0');
  return `${hours}:${minutes}`
}

export  function FormatDatewithTime (dateTime:string) {
  const date = new Date(dateTime);
  return new Intl.DateTimeFormat('en-US', {
    month: 'long',
    day: 'numeric',
    year: 'numeric',
    hour: 'numeric',
    minute: '2-digit',
    hour12: true,
  }).format(date);
};