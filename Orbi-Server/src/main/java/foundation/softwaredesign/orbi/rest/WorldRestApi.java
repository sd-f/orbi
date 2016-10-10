package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.Player;
import foundation.softwaredesign.orbi.model.Statistics;
import foundation.softwaredesign.orbi.model.World;
import foundation.softwaredesign.orbi.service.*;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.transaction.Transactional;
import javax.validation.constraints.NotNull;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;

import static java.util.Objects.isNull;
import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/world")
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
@RequestScoped
public class WorldRestApi {

    @Inject
    WorldFactory worldFactory;
    @Inject
    ElevationService elevationService;
    @Inject
    WorldService worldService;
    @Inject
    UserService userService;
    @Inject
    GameObjectService gameObjectService;
    @Inject
    CharacterService characterService;

    private void checkPlayerParameter(Player player) {
        if (isNull(player)) {
            throw new BadRequestException();
        }
        if (isNull(player.getCharacter())) {
            throw new BadRequestException();
        }
        if (isNull(player.getCharacter().getTransform())) {
            throw new BadRequestException();
        }
        if (isNull(player.getCharacter().getTransform().getGeoPosition())) {
            throw new BadRequestException();
        }
    }

    @GET
    @Path("/reset")
    @Produces(MediaType.TEXT_PLAIN)
    public String reset() {
        worldFactory.reset();
        return "OK";
    }

    @GET
    @Path("/statistics")
    public Statistics statistics() {
        Statistics statistics = new Statistics();
        statistics.setNumberOfObjects(gameObjectService.count());
        statistics.setNumberOfPlayers(characterService.count());
        return statistics;
    }

    @POST
    @Path("/terrain")
    public World terrain(@NotNull World terrain) {
        elevationService.addAltitude(terrain);
        return terrain;
    }

    @POST
    @Transactional
    @Path("/around")
    public World world(@NotNull Player player) {
        checkPlayerParameter(player);
        characterService.updateTransform(player.getCharacter().getTransform());
        return worldService.getWorld(player.getCharacter().getTransform().getGeoPosition());
    }

}
