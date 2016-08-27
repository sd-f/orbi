package foundation.softwaredesign.orbi.rest.exception.mapper;

import foundation.softwaredesign.orbi.model.exception.ErrorMessage;

import javax.ws.rs.BadRequestException;
import javax.ws.rs.NotFoundException;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;
import javax.ws.rs.ext.Provider;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Provider
public class BadRequestExceptionMapper implements ExceptionMapper<BadRequestException> {
    @Override
    public Response toResponse(BadRequestException exception) {
        ErrorMessage message = new ErrorMessage();
        message.setStatus(Response.Status.BAD_GATEWAY.getStatusCode());
        message.setMessage(exception.getMessage());
        return Response.status(Response.Status.BAD_GATEWAY).entity(message).build();
    }
}
