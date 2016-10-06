package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.service.ServerService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/server")
@Produces({APPLICATION_JSON})
@RequestScoped
public class ServerRestApi {

    @Inject
    ServerService serverService;

    @POST
    @Path("/version")
    public String version() {
        return serverService.getVersion().toString();
    }

}
