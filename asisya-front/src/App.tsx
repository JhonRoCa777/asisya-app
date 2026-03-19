import { BrowserRouter, Route } from 'react-router-dom';
import { Provider } from 'react-redux';
import { Store } from './store';
import { AuthGuard } from '@/guards';
import { RoutesWithNotFound, SuspenseLazy } from '@/components';
import { ROUTER } from '@/router';
import LoginPage from '@/pages/public/Login';

function App() {
  return (
    <Provider store={Store}>
      <BrowserRouter>
        <RoutesWithNotFound navigateTo={`${ROUTER.PRIVATE.PRODUCTS.fullPath}`}>
          <Route path={ROUTER.PUBLIC.fullPath} element={<LoginPage />} />

          <Route element={<AuthGuard />}>
            <Route path={ROUTER.PRIVATE.fullPath} element={<SuspenseLazy path={import('@/pages/private')}/>}>
              <Route path={ROUTER.PRIVATE.PRODUCTS.link.slice(1)}
                element={<SuspenseLazy path={import('@/pages/private/product')}/>}/>

              <Route path={ROUTER.PRIVATE.CATEGORIES.link.slice(1)}
                element={<SuspenseLazy path={import('@/pages/private/category')}/>}/>
            </Route>
          </Route>
        </RoutesWithNotFound>
      </BrowserRouter>
    </Provider>
  )
}

export default App
