import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Container, Form } from "react-bootstrap";
import { AuthService } from "@/services";
import { DOCUMENT_TYPE_ENUM } from "@/models";
import { SwalHelper } from "@/helpers";
import { ROUTER } from "@/router";

export default function LoginPage() {
  
  const [documentType, setDocumentType] = useState('CC');
  const [document, setDocument] = useState('');
  const { login } = AuthService();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const Resp = await login({document, documentType});
    if(Resp.data.isSuccess)
    {
      SwalHelper.success('Bienvenido');
      navigate(ROUTER.PRIVATE.PRODUCTS.fullPath);
    }
  };

  return (
    <Container style={{ maxWidth: '400px', marginTop: '50px' }}>
      <h2>Login</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3" controlId="formDocumentType">
              <Form.Label>Tipo de Documento</Form.Label>
              <Form.Select
                value={documentType}
                onChange={(e) => setDocumentType(e.target.value)}
              >
                {Object.entries(DOCUMENT_TYPE_ENUM).map(([key, value]) => (
                  <option key={key} value={value}>
                    {value}
                  </option>
                ))}
              </Form.Select>
            </Form.Group>

        <Form.Group className="mb-3" controlId="formDocument">
          <Form.Label>Documento "Ingrese 1005772426"</Form.Label>
          <Form.Control
            placeholder="Ingresa Documento"
            value={document}
            onChange={(e) => setDocument(e.target.value)}
          />
        </Form.Group>

        <Button variant="primary" type="submit">
          Ingresar
        </Button>
      </Form>
    </Container>
  );
}
