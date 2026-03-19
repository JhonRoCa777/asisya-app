import { Spinner } from 'react-bootstrap';
import style from './index.module.css';

export function SpinnerFullScreen() {
  return (
    <div className={style.FullScreenCentered}>
      <Spinner animation='border' variant='secondary'/>
    </div>
  )
}
