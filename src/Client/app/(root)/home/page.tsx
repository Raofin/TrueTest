'use client';

import { Card, CardHeader, CardBody, Button } from '@heroui/react';
import { Icon } from '@iconify/react/dist/iconify.js';
import { useState } from 'react';
import RootNavBar from '../root-navbar';

const states = [
  { id: 1, icon: 'fa-solid:camera', title: 'Camera' },
  { id: 2, icon: 'fa-solid:microphone-alt', title: 'Microphone' },
  { id: 3, icon: 'fa-solid:desktop', title: 'Screen Recording' },
  { id: 4, icon: 'fa-solid:clipboard-list', title: 'Clipboard' },
];

export default function Component() {
  const [permit, setPermit] = useState(false);

  const requestPermissions = async () => {
    try {
      if (document.visibilityState !== 'visible') {
        alert('Please keep the tab active and focused when granting permissions.');
        return;
      }
      await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      await navigator.mediaDevices.getDisplayMedia({ video: true });
      setPermit(true);
    } catch (err) {
      console.error('Permission denied or failed:', err);
      alert('Permission denied or failed: ' + (err as Error).message);
      setPermit(false);
    }
  };
  return (
    <>
      <RootNavBar />
      <div className="flex flex-col gap-6 items-center justify-center text-center h-full w-full">
        <div className="gap-5 flex">
          {states.map((stat) => (
            <Card
              key={stat.id}
              className="py-4 w-[200px] text-center flex flex-col items-center shadow-none bg-white dark:bg-[#18181b]"
            >
              <CardHeader className="pb-0 pt-2 px-4 flex flex-col items-center">
                <Icon icon={stat.icon} width={50} height={50} />
              </CardHeader>
              <CardBody className="py-2 mt-4 text-center">
                <h2 className="text-lg font-semibold">{stat.title}</h2>
              </CardBody>
            </Card>
          ))}
        </div>
        <p className="w-[800px] text-center mt-5 text-gray-700 dark:text-gray-300">
  To provide you with an immersive and interactive experience, our application 
  requires access to key system features. Granting camera and microphone permissions
   allows us to enable real-time video and voice communication, making virtual interactions 
   feel more engaging. Screen recording permission is essential for capturing on-screen activities. Clipboard access is requested to enhance productivity by enabling quick copy-paste actions, especially when working
     with shared data or text inputs.
</p>
        <div>
          <Button
            color="primary"
            className="my-10 w-[200px] ml-10"
            onPress={requestPermissions}
            isDisabled={permit}
          >
            {permit ? 'Permission Granted' : 'Grant Permission'}
          </Button>
        </div>
      </div>
    </>
  );
}
