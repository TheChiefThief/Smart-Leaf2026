import { Navigate } from "react-router-dom";
import Cookies from 'js-cookie';


function withSessionCheck({ children}) {
   
    let usuarioCookie = Cookies.get('userData');
    console.log("cookies:",usuarioCookie)
    if( usuarioCookie){
        usuarioCookie = JSON.parse(usuarioCookie)
        if (usuarioCookie.username==""){
            return <Navigate to="../"/>
        }
    } else {
        return <Navigate to="../"/>
        }
     
     return children;

}

export default withSessionCheck;