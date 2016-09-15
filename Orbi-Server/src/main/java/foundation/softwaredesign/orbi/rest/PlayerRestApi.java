package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.Inventory;
import foundation.softwaredesign.orbi.model.Player;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.service.ElevationService;
import foundation.softwaredesign.orbi.service.PlayerService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.transaction.Transactional;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/player")
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
@RequestScoped
public class PlayerRestApi {

    @Inject
    ElevationService elevationService;
    @Inject
    PlayerService playerService;

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
        return playerService.craft(player);
    }


    @POST
    @Path("/destroy")
    @Transactional
    public World delete(@NotNull Player player) {
        return playerService.destroy(player);
    }

    @GET
    @Path("/inventory")
    public Inventory inventory() {
        return playerService.getInventory();
    }

}
