'use client'

type FormatTimeProps = {
  minute: number;
};
export default function  FormatTime({minute}:FormatTimeProps) {
  const hours = Math.floor(minute / 60)
  const minutes =Math.floor(minute % 60).toString().padStart(2, '0');
  return `${hours}:${minutes}`
}