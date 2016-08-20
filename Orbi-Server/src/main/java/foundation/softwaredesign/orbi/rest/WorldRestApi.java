package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.service.ElevationService;
import foundation.softwaredesign.orbi.service.GameObjectService;
import foundation.softwaredesign.orbi.service.WorldFactory;
import foundation.softwaredesign.orbi.service.WorldService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;

import java.util.List;

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

    @GET
    @Path("/altitude")
    public World terrain(@NotNull World terrain) {
        elevationService.addAltitude(terrain);
        return terrain;
    }

    @GET
    @Path("/around")
    public World world(@NotNull GeoPosition geoPosition) {
        return worldService.getWorld(geoPosition);
    }


}
