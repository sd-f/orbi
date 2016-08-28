package foundation.softwaredesign.orbi.rest.filter;

import foundation.softwaredesign.orbi.rest.exception.VersionNotSupportedException;
import foundation.softwaredesign.orbi.service.ServerService;

import javax.annotation.Priority;
import javax.ws.rs.BadRequestException;
import javax.ws.rs.Consumes;
import javax.ws.rs.Produces;
import javax.ws.rs.container.ContainerRequestContext;
import javax.ws.rs.container.ContainerRequestFilter;
import javax.ws.rs.ext.Provider;
import java.io.IOException;
import java.util.Objects;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Provider
@Priority(value = 1)
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
public class ServerVersionFilter implements ContainerRequestFilter {

    @Override
    public void filter(ContainerRequestContext requestContext) throws IOException {
        String versionString = requestContext.getHeaderString("X-App-Version");
        if (Objects.isNull(versionString)) {
            throw new BadRequestException("App version missing");
        }
        Long version = new Long(versionString);
        if (Objects.isNull(version)) {
            throw new VersionNotSupportedException("App version parse error");
        }
        new ServerService().checkVersion(version);
    }
}
