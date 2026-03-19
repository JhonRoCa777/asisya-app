import { SwalHelper } from "@/helpers";
import { ROUTER } from "@/router";
import { AuthService } from "@/services";
import { Button, Container, Nav, Navbar } from "react-bootstrap";
import { FiLogOut } from "react-icons/fi";
import { Link, Outlet, useNavigate } from "react-router-dom";

export default function PrivatePage() {
    const navigate = useNavigate();
    const { logout } = AuthService();

    const handleLogout = async() => {
        await logout();
        SwalHelper.success('Hasta pronto');
        navigate(ROUTER.PUBLIC.fullPath);
    };

    return (
        <>
            <Navbar bg="dark" variant="dark" expand="lg">
                <Container>
                    <Navbar.Brand>Mi App</Navbar.Brand>

                    <Navbar.Toggle aria-controls="main-navbar" />

                    <Navbar.Collapse id="main-navbar">
                        <Nav>
                            <Nav.Link as={Link} to={ROUTER.PRIVATE.PRODUCTS.link.slice(1)}> Productos </Nav.Link>
                            <Nav.Link as={Link} to={ROUTER.PRIVATE.CATEGORIES.link.slice(1)}> Categorías </Nav.Link>
                        </Nav>

                        <Button
                            variant="outline-light"
                            onClick={handleLogout}
                            className="d-flex align-items-center gap-2"
                        >
                            <FiLogOut />
                            Logout
                        </Button>
                    </Navbar.Collapse>
                </Container>
            </Navbar>

            <Container fluid className="mt-4">
                <Outlet />
            </Container>
        </>
    );
}