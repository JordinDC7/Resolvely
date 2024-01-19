import securedRoutes from './securedRoutes';
import publicRoutes from './publicRoutes';

// flatten the list of all nested routes
const flattenRoutes = routes => {
  let flatRoutes = [];

  routes = routes || [];
  routes.forEach(item => {
    flatRoutes.push(item);
    if (typeof item.children !== 'undefined') {
      flatRoutes = [...flatRoutes, ...flattenRoutes(item.children)];
    }
  });
  return flatRoutes;
};

const authProtectedFlattenRoutes = flattenRoutes([...securedRoutes]);
const publicProtectedFlattenRoutes = flattenRoutes([...publicRoutes]);
export { authProtectedFlattenRoutes, publicProtectedFlattenRoutes };
