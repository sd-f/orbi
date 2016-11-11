package foundation.softwaredesign.orbi.rest.filter;

import foundation.softwaredesign.orbi.model.exception.ErrorMessage;
import foundation.softwaredesign.orbi.persistence.entity.IdentityEntity;
import foundation.softwaredesign.orbi.persistence.repo.auth.IdentityRepository;
import foundation.softwaredesign.orbi.service.auth.IdentityThreadLocal;

import javax.annotation.Priority;
import javax.inject.Inject;
import javax.persistence.NoResultException;
import javax.ws.rs.Consumes;
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

    @Inject
    IdentityRepository identityRepository;

    @Override
    public void filter(ContainerRequestContext requestContext) throws IOException {
        IdentityThreadLocal.set(null);
        String path = requestContext.getUriInfo().getPath();
        if (path.equals("server/version") || path.equals("server/info") || path.equals("auth/login") || path.equals("auth/requestcode")) {
            return;
        }
        String bearerString = requestContext.getHeaderString("Authorization");
        if (Objects.isNull(bearerString)) {
            abort(requestContext, "Please log in");
            return;
        }
        String[] berearStringSplitted = bearerString.split(" ");
        if (Objects.isNull(berearStringSplitted) || (berearStringSplitted.length < 2)) {
            abort(requestContext, "Please log in");
            return;
        }
        if (Objects.isNull(berearStringSplitted[0]) || Objects.isNull(berearStringSplitted[1])) {
            abort(requestContext, "Please log in");
            return;
        }
        if (berearStringSplitted[0].isEmpty() || berearStringSplitted[1].isEmpty()) {
            abort(requestContext, "Please log in");
            return;
        }
        IdentityEntity identityEntity = null;
        try {
            identityEntity = identityRepository.findByToken(berearStringSplitted[1]);

        } catch (NoResultException|NullPointerException|IllegalStateException ex) {
            abort(requestContext, "Please log in");
        }
        IdentityThreadLocal.set(identityEntity);
    }

    private void abort(ContainerRequestContext requestContext, String text) {
        ErrorMessage message = new ErrorMessage();
        message.setStatus(Response.Status.UNAUTHORIZED.getStatusCode());
        message.setMessage("Unauthorized - " + text);
        requestContext.abortWith(Response
                .status(Response.Status.UNAUTHORIZED)
                .header(HttpHeaders.CONTENT_TYPE, MediaType.APPLICATION_JSON)
                .entity(message)
                .build());
    }
}
