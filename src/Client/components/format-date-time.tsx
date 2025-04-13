
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
