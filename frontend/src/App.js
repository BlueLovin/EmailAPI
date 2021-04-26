import './App.css';
import {useState, useEffect} from 'react';
import {Form, FormGroup, Input, Button, Container, Alert, Jumbotron} from 'reactstrap';

function App() {
  const [activeItem, setActiveItem] = useState({
    To: '',
    Subject: '',
    Body: '',
    Attachments: null,
  });
  const protocol = process.env.REACT_APP_API_SSL_ENABLED === '1' ? "https" : "http" 
  const apiSendEndpoint = protocol + '://localhost:' + process.env.REACT_APP_EMAILAPI_PORT + '/api/mail/send';
  const [isLoading, setIsLoading] = useState(false);
  const [isSent, setIsSent] = useState(false);
  const [failed, setFailed] = useState(false);
  const [sentAlertVisible, setSentAlertVisible] = useState(false);
  const [failedAlertVisible, setFailedAlertVisible] = useState(false);

  useEffect(() => {
    console.log(apiSendEndpoint)
    
    console.log(protocol)
  }, [isLoading]);

  const showSentAlert = () => {
    setSentAlertVisible(true, window.setTimeout(()=>{
      setSentAlertVisible(false);
      setIsSent(false);
    }, 5000));
  }
  const showFailedAlert = () => {
    setFailedAlertVisible(true, window.setTimeout(()=>{
      setFailedAlertVisible(false);
      setFailed(false);
    }, 15000));
  }

const handleChange = (e) => {
    let { name, value } = e.target;

    const item = {
        ...activeItem,
        [name]: value,
    };
    if(failed){
      setFailed(false);
    }
    setActiveItem(item);
}

	const changeAttachmentsHandler = (event) => {
    const item = {...activeItem,
         Attachments: event.target.files[0],
     };
     
		setActiveItem(item);
	};

	const handleSubmission = async () => {
    if(!isLoading){
      setIsLoading(true);
      const formData = new FormData();

      formData.append('Recipient', activeItem.To);
      formData.append('Subject', activeItem.Subject);
      formData.append('Body', activeItem.Body);
      formData.append('Attachments', activeItem.Attachments);

      await fetch(
        apiSendEndpoint,
        {
          method: 'POST',
          body: formData,
        }
      )
        .then((response) => response.json())
        .then((response) => {
            setIsSent(true);
            showSentAlert();
        })
        .catch((error) => {
            setFailed(true);
            showFailedAlert();
        });

        setIsLoading(false);

        setActiveItem({
          To: '',
          Subject: '',
          Body: '',
          Attachments: null,
        });
    }
	};

  return (
    <Jumbotron className="vertical-center bg-dark">
      <Container className="bg-secondary text-white text-center">
        <br/>
      <h1 className="display-3">Clarity Skills Assessment</h1>
      <h4>By Matthew Jury</h4>
      <blockquote>To whom it may concern, find the hidden element on this page!</blockquote>
        <br/>
        <Form>
          <FormGroup>
            <Input type="email" name="To"  
            placeholder="example@example.com" 
            onChange={handleChange} 
            value={activeItem.To.trim()/*no spaces in recipient allowed*/}/> 
          </FormGroup>
          <FormGroup>
            <Input type="text" name="Subject" placeholder="Subject" onChange={handleChange} value={activeItem.Subject} />
          </FormGroup>
          <FormGroup>
            <Input type="textarea" name="Body" placeholder="Your Email here" onChange={handleChange} value={activeItem.Body}/>
          </FormGroup>
          <FormGroup>
            <Input type="file" name="Attachments" onChange={changeAttachmentsHandler} />
          </FormGroup>
          <Button className="btn-success btn-lg" 
          onClick={handleSubmission} 
          >
            {isLoading ? "Sending" : "Send"}
          </Button>
          
          <br/>
          <br/>
          {isSent ? 
            <Alert color="primary" isOpen={sentAlertVisible}>
              Successfully sent E-Mail
            </Alert>
          : null}

          {failed ? 
            <Alert color="danger" isOpen={failedAlertVisible}>
              Could not send E-Mail. Try again.
            </Alert>
          : null}
        </Form>
        <br/>
      <h4 className="text-muted">Hire me! :)</h4>
      </Container>
    </Jumbotron>
  );
}

export default App;