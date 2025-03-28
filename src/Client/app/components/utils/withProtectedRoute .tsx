'use client'

import React, { useEffect, ComponentType } from 'react';
import { useRouter } from 'next/navigation';
import { getAuthToken } from './auth'

const withProtectedRoute = <P extends object>(WrappedComponent: ComponentType<P>) => {
  const ProtectedComponent = (props: P) => {
    const router = useRouter();

    useEffect(() => {
      if (!getAuthToken()) {
        router.replace('/login');
      }
    }, [router]);

    return getAuthToken() ? <WrappedComponent {...props} /> : null;
  };

  ProtectedComponent.displayName = `WithProtectedRoute(${WrappedComponent.displayName || WrappedComponent.name || 'Component'})`;

  return ProtectedComponent;
};

export default withProtectedRoute;
