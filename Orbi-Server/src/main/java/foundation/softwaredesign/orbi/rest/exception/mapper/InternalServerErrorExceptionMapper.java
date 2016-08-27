package foundation.softwaredesign.orbi.rest.exception.mapper;

import com.sun.media.sound.InvalidDataException;
import foundation.softwaredesign.orbi.model.exception.ErrorMessage;

import javax.ws.rs.InternalServerErrorException;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;
import javax.ws.rs.ext.Provider;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Provider
public class InternalServerErrorExceptionMapper implements ExceptionMapper<InternalServerErrorException> {

    @Override
    public Response toResponse(InternalServerErrorException exception) {
        ErrorMessage message = new ErrorMessage();
        message.setStatus(Response.Status.INTERNAL_SERVER_ERROR.getStatusCode());
        message.setMessage(exception.getMessage());
        return Response.status(Response.Status.INTERNAL_SERVER_ERROR).entity(message).build();
    }

}
