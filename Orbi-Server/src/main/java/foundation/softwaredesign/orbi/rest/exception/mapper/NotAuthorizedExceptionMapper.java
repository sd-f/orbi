package foundation.softwaredesign.orbi.rest.exception.mapper;

import foundation.softwaredesign.orbi.model.exception.ErrorMessage;

import javax.ws.rs.Consumes;
import javax.ws.rs.NotAuthorizedException;
import javax.ws.rs.Produces;
import javax.ws.rs.core.HttpHeaders;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;
import javax.ws.rs.ext.Provider;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Provider
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
public class NotAuthorizedExceptionMapper implements ExceptionMapper<NotAuthorizedException> {
    @Override
    public Response toResponse(NotAuthorizedException exception) {
        ErrorMessage message = new ErrorMessage();
        message.setStatus(Response.Status.UNAUTHORIZED.getStatusCode());
        message.setMessage(exception.getMessage());
        return Response
                .status(Response.Status.UNAUTHORIZED)
                .header(HttpHeaders.CONTENT_TYPE, MediaType.APPLICATION_JSON)
                .entity(message)
                .build();
    }
}