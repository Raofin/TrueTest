'use client'

import React, { useEffect, ComponentType } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/app/context/AuthProvider'

const withProtectedRoute = <P extends object>(WrappedComponent: ComponentType<P>) => {
  const ProtectedComponent = (props: P) => {
    const { user } = useAuth();
    const router = useRouter();

    useEffect(() => {
      if (!user) {
        router.replace('/login');
      }
    }, [user, router]);

    return user ? <WrappedComponent {...props} /> : null;
  };

  ProtectedComponent.displayName = `WithProtectedRoute(${
    WrappedComponent.displayName || WrappedComponent.name || 'Component'
  })`;

  return ProtectedComponent;
};

export default withProtectedRoute;
