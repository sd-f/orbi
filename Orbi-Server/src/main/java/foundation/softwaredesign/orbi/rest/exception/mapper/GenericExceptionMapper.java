package foundation.softwaredesign.orbi.rest.exception.mapper;

import foundation.softwaredesign.orbi.model.exception.ErrorMessage;

import javax.annotation.Priority;
import javax.transaction.TransactionalException;
import javax.ws.rs.Consumes;
import javax.ws.rs.Produces;
import javax.ws.rs.WebApplicationException;
import javax.ws.rs.core.HttpHeaders;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;
import javax.ws.rs.ext.Provider;
import java.util.logging.Level;
import java.util.logging.Logger;

import static java.util.Objects.nonNull;
import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Provider
@Priority(100)
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
public class GenericExceptionMapper implements ExceptionMapper<Exception> {

    @Override
    public Response toResponse(Exception exception) throws WebApplicationException {
        ErrorMessage message = new ErrorMessage();
        message.setStatus(Response.Status.INTERNAL_SERVER_ERROR.getStatusCode());
        message.setMessage("Internal Server Error");

        if (exception instanceof TransactionalException) {
            if (nonNull(exception.getCause()) && (exception.getCause() instanceof WebApplicationException)){
                WebApplicationException originalException = (WebApplicationException) exception.getCause();
                message.setStatus(originalException.getResponse().getStatus());
                message.setMessage(originalException.getMessage());
            }
        }

        Logger.getLogger(this.getClass().getName()).log(Level.SEVERE, exception.getMessage(), exception);
        return Response
                .status(Response.Status.INTERNAL_SERVER_ERROR)
                .header(HttpHeaders.CONTENT_TYPE, MediaType.APPLICATION_JSON)
                .entity(message)
                .build();
    }
}
