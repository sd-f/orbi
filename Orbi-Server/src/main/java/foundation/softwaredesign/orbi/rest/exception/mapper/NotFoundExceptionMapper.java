package foundation.softwaredesign.orbi.rest.exception.mapper;

import foundation.softwaredesign.orbi.model.exception.ErrorMessage;

import javax.ws.rs.InternalServerErrorException;
import javax.ws.rs.NotFoundException;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;
import javax.ws.rs.ext.Provider;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Provider
public class NotFoundExceptionMapper implements ExceptionMapper<NotFoundException> {
    @Override
    public Response toResponse(NotFoundException exception) {
        ErrorMessage message = new ErrorMessage();
        message.setStatus(Response.Status.NOT_FOUND.getStatusCode());
        message.setMessage(exception.getMessage());
        return Response.status(Response.Status.NOT_FOUND).entity(message).build();
    }
}
