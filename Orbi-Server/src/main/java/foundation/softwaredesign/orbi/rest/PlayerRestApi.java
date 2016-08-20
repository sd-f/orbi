package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.CraftingGameObject;
import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.GeoPosition;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.service.ElevationService;
import foundation.softwaredesign.orbi.service.GameObjectService;
import foundation.softwaredesign.orbi.service.PlayerService;

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

    @POST
    @Path("/altitude")
    public GeoPosition elevation(@NotNull GeoPosition geoPosition) {
        elevationService.addAltitude(geoPosition);
        return geoPosition;
    }

    @POST
    @Path("/craft")
    @Transactional
    public World create(@NotNull CraftingGameObject craftingGameObject) {
        return playerService.craftGameObject(craftingGameObject);
    }

}
