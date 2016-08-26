package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.Player;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.service.ElevationService;
import foundation.softwaredesign.orbi.service.WorldFactory;
import foundation.softwaredesign.orbi.service.WorldService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.transaction.Transactional;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;
import static javax.ws.rs.core.MediaType.APPLICATION_XML;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/world")
@Produces({APPLICATION_XML, APPLICATION_JSON})
@Consumes({APPLICATION_XML, APPLICATION_JSON})
@RequestScoped
public class WorldRestApi {

    @Inject
    WorldFactory worldFactory;
    @Inject
    ElevationService elevationService;
    @Inject
    WorldService worldService;

    @GET
    @Path("/reset")
    @Produces(MediaType.TEXT_PLAIN)
    public String reset() {
        worldFactory.reset();
        return "OK";
    }

    @POST
    @Path("/terrain")
    public World terrain(@NotNull World terrain) {
        elevationService.addAltitude(terrain);
        return terrain;
    }

    @POST
    @Path("/around")
    public World world(@NotNull Player player) {
        return worldService.getWorld(player.getGeoPosition());
    }

    @POST
    @Path("/objects/destroy")
    @Transactional
    public World delete(@NotNull Player player) {
        return worldService.delete(player);
    }


}
