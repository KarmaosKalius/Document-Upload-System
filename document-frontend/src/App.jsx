import { useEffect, useState } from "react";
function App(){
  const [documents,setDocuments] = useState([]);
  const [file, setFile] = useState(null);
  const API = "http://localhost:5280/api/document";

  async function fetchDocument(){
    const res = await fetch(`${API}/list`);
    const data = await res.json();
    setDocuments(data);
  }
  async function uploadDocument(){
    const formData = new FormData();
    formData.append("file", file);
    await fetch(`${API}/upload`,{
      method: "POST",
      body: formData
    });
    fetchDocument();
  }
  function viewDocument(id) {
    window.open(`${API}/view/${id}`, "_blank");
  }
  function downloadDocument(id){
    window.open(`${API}/download/${id}`, "_blank");
  }
  useEffect(()=>{
    fetchDocument();
  },[]);

  return(
    <>
    <div style={{padding:"40px"}}>
      <h2>Document Upload System</h2>
      <input type="file"
      onChange={(e)=>setFile(e.target.files[0])}
      />
      <button onClick={uploadDocument}>
        Upload
      </button>

      <hr/>

      <table border="1" cellPadding="10">
        <thead>
          <tr>
            <th>Id</th>
            <th>File Name</th>
            <th>Size</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {documents.map(doc =>(
            <tr key={doc.id}>
              <td>{doc.id}</td>
              <td>{doc.filesName}</td>
              <td>{doc.size}</td>
              <td>
                <button onClick={()=> viewDocument(doc.id)}>
                  view
                </button>
                <button onClick={() => downloadDocument(doc.id)}>
                  download
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    </>
  )
}

export default App;