package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.*;
import foundation.softwaredesign.orbi.service.ElevationService;
import foundation.softwaredesign.orbi.service.GameObjectService;
import foundation.softwaredesign.orbi.service.PlayerService;
import foundation.softwaredesign.orbi.service.WorldService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.transaction.Transactional;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;
import static javax.ws.rs.core.MediaType.APPLICATION_XML;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/player")
@Produces({APPLICATION_XML, APPLICATION_JSON})
@Consumes({APPLICATION_XML, APPLICATION_JSON})
@RequestScoped
public class PlayerRestApi {

    @Inject
    ElevationService elevationService;

    @Inject
    PlayerService playerService;

    @Inject
    GameObjectService gameObjectService;

    @Inject
    WorldService worldService;

    @POST
    @Path("/altitude")
    public Player elevation(@NotNull Player player) {
        elevationService.addAltitude(player.getGeoPosition());
        return player;
    }

    @POST
    @Path("/craft")
    @Transactional
    public World create(@NotNull Player player) {
        return playerService.craftGameObject(player);
    }

}
