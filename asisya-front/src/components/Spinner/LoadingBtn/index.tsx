import { Spinner } from "react-bootstrap";
import styles from "./index.module.css";

export function SpinnerLoadingBtn() {
  return (
    <div className={styles.Container}>
      <Spinner size="sm"/>
    </div>
  )
}
