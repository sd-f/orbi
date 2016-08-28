package foundation.softwaredesign.orbi.rest.exception;

import javax.ws.rs.WebApplicationException;
import javax.ws.rs.core.Response;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class VersionNotSupportedException extends WebApplicationException {

    public VersionNotSupportedException(String message) {
        super(message, Response.Status.HTTP_VERSION_NOT_SUPPORTED);
    }
}
