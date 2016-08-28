package foundation.softwaredesign.orbi.rest.filter;

import foundation.softwaredesign.orbi.model.exception.ErrorMessage;
import foundation.softwaredesign.orbi.service.authorization.TokenThreadLocal;

import javax.annotation.Priority;
import javax.ws.rs.BadRequestException;
import javax.ws.rs.Consumes;
import javax.ws.rs.NotAuthorizedException;
import javax.ws.rs.Produces;
import javax.ws.rs.container.ContainerRequestContext;
import javax.ws.rs.container.ContainerRequestFilter;
import javax.ws.rs.container.PreMatching;
import javax.ws.rs.core.HttpHeaders;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.Provider;
import java.io.IOException;
import java.util.Objects;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Provider
@PreMatching
@Priority(value = 2)
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
public class AuthorizationFilter implements ContainerRequestFilter {

    @Override
    public void filter(ContainerRequestContext requestContext) throws IOException {
        String path = requestContext.getUriInfo().getPath();
        if (path.equals("server/version") || path.equals("auth/login")) {
            return;
        }
        String bearerString = requestContext.getHeaderString("Authorization");
        if (Objects.isNull(bearerString)) {
            abort(requestContext);
            return;
        }
        String[] berearStringSplitted = bearerString.split(" ");
        if (Objects.isNull(berearStringSplitted) || (berearStringSplitted.length < 2)) {
            abort(requestContext);
            return;
        }
        if (Objects.isNull(berearStringSplitted[0]) || Objects.isNull(berearStringSplitted[1])) {
            abort(requestContext);
            return;
        }
        if (berearStringSplitted[0].isEmpty() || berearStringSplitted[1].isEmpty()) {
            abort(requestContext);
            return;
        }

        TokenThreadLocal.set(berearStringSplitted[1]);
    }

    private void abort(ContainerRequestContext requestContext) {
        ErrorMessage message = new ErrorMessage();
        message.setStatus(Response.Status.UNAUTHORIZED.getStatusCode());
        message.setMessage("Unauthorized - Please log in");
        requestContext.abortWith(Response
                .status(Response.Status.UNAUTHORIZED)
                .header(HttpHeaders.CONTENT_TYPE, MediaType.APPLICATION_JSON)
                .entity(message)
                .build());
    }
}
