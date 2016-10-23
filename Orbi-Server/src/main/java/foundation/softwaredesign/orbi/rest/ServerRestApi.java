package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.game.server.ServerInfo;
import foundation.softwaredesign.orbi.service.game.server.ServerService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.GET;
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

    @GET
    @Path("/info")
    public ServerInfo info() {
        return serverService.loadInfo();
    }

}
